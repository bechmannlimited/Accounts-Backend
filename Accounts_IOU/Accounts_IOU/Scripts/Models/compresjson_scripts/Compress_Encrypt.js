
function Compress(str) {
    var rc = LZString.compress(str);
    return Base64.encode(rc);
}

function Decompress(str) {
    var rc = Base64.decode(str);
    return LZString.decompress(rc);
}

function Encrypt(str, key) {
    var encrypted = CryptoJS.AES.encrypt(str, key); 
    return encrypted.toString();
}

function Decrypt(str, key) {
    var encrypted = CryptoJS.AES.decrypt(str, key);
    return encrypted.toString(CryptoJS.enc.Utf8);
}

// JS TEMP ONES: to include BASE64
function JSEncrypt(str, key) {
    var encrypted = CryptoJS.AES.encrypt(str, key);
    return Base64.encode(encrypted.toString());
}

function JSDecrypt(str, key) {
    var s = Base64.decode(str);
    var encrypted = CryptoJS.AES.decrypt(s, key);
    return encrypted.toString(CryptoJS.enc.Utf8);
}

function EncryptAndCompress(str, key) {
    return JSEncrypt(Compress(str), key);
}

function DecryptAndDecompress(str, key) {
    //return Decompress(JSDecrypt(str, key));
    return JXG.decompress(JSDecrypt(str, key));
}