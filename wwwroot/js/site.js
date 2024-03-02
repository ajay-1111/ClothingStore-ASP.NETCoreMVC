// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

window.addEventListener('beforeunload', function (e) {
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Account/Logout', true);
    xhr.setRequestHeader('Content-Type', 'application/json');
    xhr.send();
});
