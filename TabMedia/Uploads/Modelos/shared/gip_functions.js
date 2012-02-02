function changePage(pagina){
    window.location = "?cmd=changePage&page="+pagina;
}

function openAttachment(attachment) {
    window.location = "?cmd=openAttachment&pId=" + attachment;
}

function logMessage(message) {
    window.location = "?cmd=log&message=" + message;
}