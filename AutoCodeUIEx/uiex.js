var getUrl = function (url) {
    var httpRequest = new XMLHttpRequest();//第一步：建立所需的对象
    httpRequest.open('GET', url, true);//第二步：打开连接 
    httpRequest.send();
}

var gen = function (url) {
    if (url.indexOf("http") == -1) {
        url = window.location.origin + url
    }

    location.href = "AutoGenCode://" + url;
}

window.onload = function () {

    setTimeout(() => {
        var apiUrl = document.querySelector(".main > a").href;

        var mainBox = document.querySelector(".main")

        var one = document.createElement("div");
        one.innerHTML = `<a href="javascript:void(0)" onclick="gen('${apiUrl}')">自动生成代码</a>`
        mainBox.appendChild(one);

        //var two = document.createElement("div");
        //var url2 = `/api/generate?herf=${apiUrl}&lang=javascript`;
        //two.innerHTML = `<a href='#' onclick='getUrl("${url2}")'>自动生成冯敬涛专用代码</a>`
        //mainBox.appendChild(two);

    }, 2000)
}();