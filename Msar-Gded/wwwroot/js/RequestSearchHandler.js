document.getElementById('searchBox').addEventListener('input', function () {


    let controllerName = document.getElementById('controllerName').value;
    let keyword = this.value.trim(); // Remove spaces
    const statusId = document.getElementById('statusId').value;
    const typeOfRequestId = document.getElementById('typeOfRequestId').value;
    let baseUrl = document.getElementById('baseUrl').value;


    // alert(`Badr + ${baseUrl} + badr`);

    fetch(`${baseUrl}${controllerName}/Search?keyword=${encodeURIComponent(keyword)}&statusId=${statusId}&typeOfRequestId=${typeOfRequestId}`)
        .then(response => response.text())
        .then(data => {
            let tableBody = document.getElementById('entityTableBody');

            if (keyword === '') {
                // If input is empty, reload all entities
                fetch(`${baseUrl}${controllerName}/Search?keyword=`)
                    .then(response => response.text())
                    .then(data => {
                        tableBody.innerHTML = data;
                    });
            } else {
                tableBody.innerHTML = data.trim() ? data : '<tr><td colspan="2" class="text-center">لم يتم العثور على نتائج.</td></tr>';
            }
        });

});

//document.getElementById('searchBox').addEventListener('input', function () {
//    let controllerName = document.getElementById('controllerName').value;
//    let keyword = this.value.trim();
//    let baseUrl = document.getElementById('baseUrl').value;

//    const requestDataJson = document.getElementById('requestDataJson');
//    const requests = JSON.parse(requestDataJson.textContent);


//    fetch(`${baseUrl}${controllerName}/Search?keyword=${encodeURIComponent(keyword)}`, {
//        method: 'POST',
//        headers: {
//            'Content-Type': 'application/json'
//        },
//        body: JSON.stringify(requests)
//    })
//        .then(response => response.text())
//        .then(data => {
//            let tableBody = document.getElementById('entityTableBody');

//            tableBody.innerHTML = data.trim()
//                ? data
//                : '<tr><td colspan="9" class="text-center">لم يتم العثور على نتائج.</td></tr>';
//        });
//});
