function GetUrl(targetUrl) {
    return "http://localhost:9091/" + targetUrl;
}

//date
function appendZero(s) {
    return ("00" + s).substr((s + "").length);
}  //补0函数