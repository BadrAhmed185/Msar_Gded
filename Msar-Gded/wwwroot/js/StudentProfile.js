document.addEventListener('DOMContentLoaded', function () {

    const viewButtons = document.querySelectorAll('.view-file');
    const deleteButtons = document.querySelectorAll('.delete-file');
    const fileInputs = document.querySelectorAll('input[type="file"]');
    const previewImage = document.getElementById('previewImage');

    viewButtons.forEach(button => {
        button.addEventListener('click', function () {
            const fileInput = this.closest('.file-upload').querySelector('input[type="file"]');
            const fileUrl = this.getAttribute('data-url');

            if (fileInput.files.length > 0) {
                // Preview newly selected file
                const file = fileInput.files[0];
                const reader = new FileReader();

                reader.onload = function (e) {
                    previewImage.src = e.target.result;
                    const previewModal = new bootstrap.Modal(document.getElementById('filePreviewModal'));
                    previewModal.show();
                };

                reader.readAsDataURL(file);
            } else if (fileUrl) {
                // Preview existing file from server
                previewImage.src = fileUrl;
                const previewModal = new bootstrap.Modal(document.getElementById('filePreviewModal'));
                previewModal.show();
            } else {
                alert('لم يتم اختيار ملف للعرض.');
            }
        });
    });

});