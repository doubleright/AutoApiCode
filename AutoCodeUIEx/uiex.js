var getUrl = function (url) {
    var httpRequest = new XMLHttpRequest();//第一步：建立所需的对象
    httpRequest.open('GET', url, true);//第二步：打开连接 
    httpRequest.send();
}

var gen = function () {

    var url = document.querySelector("#select").firstChild.value;
    if (url.indexOf("http") == -1) {
        url = window.location.origin + url
    }

    location.href = "AutoGenCode://" + url;
}

window.onload = function () {

    setTimeout(() => {
        var img = document.createElement("img");
        img.src = logo;
        img.addEventListener("click", gen);
        img.style = "height:40px;width:40px;margin-left:10px"

        var warp = document.querySelector(".select-label");
        warp.appendChild(img);

    }, 2000)
}();