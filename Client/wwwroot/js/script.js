function triggerFileDownload(fileName, url) {

    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();

}

function downloadFileFromStream(fileName, streamRef) {
    streamRef.openRead().then(function (reader) {
        const blob = new Blob([reader.result]);
        const link = document.createElement('a');
        link.href = URL.createObjectURL(blob);
        link.download = fileName;
        link.click();
    });
};