window.onload = function () {
    var el = document.getElementById("privacy-policy-ok-button");
    el.onclick = hidePrivacy;
}

function hidePrivacy() {
    document.getElementById("footer-privacy-policy").classList.add("d-none");
}