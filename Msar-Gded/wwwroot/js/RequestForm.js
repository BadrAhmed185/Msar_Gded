document.addEventListener('DOMContentLoaded', function () {
    const submitButton = document.getElementById('submitButton');
    const acceptTermsButton = document.getElementById('acceptTerms');
    const viewButtons = document.querySelectorAll('.view-file');
    const deleteButtons = document.querySelectorAll('.delete-file');
    const fileInputs = document.querySelectorAll('input[type="file"]');
    const previewImage = document.getElementById('previewImage');



    // Enable Submit Button after accepting terms
    acceptTermsButton.addEventListener('click', function () {
        submitButton.disabled = false;
    });

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


    // Handle File Delete Buttons
    deleteButtons.forEach((button, index) => {
        button.addEventListener('click', function () {
            const fileInput = fileInputs[index];
            fileInput.value = ''; // Clear file input
        });
    });

    // Auto-calculate "Total Degree" if user edits degrees
    const pureDegreeInput = document.getElementById('PureDegree');
    const degreeWithLanguageInput = document.getElementById('DegreeWithLanguage');
    const totalDegreeInput = document.getElementById('TotalDegree');

    function calculateTotalDegree() {
        const pureDegree = parseFloat(pureDegreeInput.value) || 0;
        const degreeWithLanguage = parseFloat(degreeWithLanguageInput.value) || 0;
        totalDegreeInput.value = (pureDegree + degreeWithLanguage).toFixed(2);
    }

    pureDegreeInput.addEventListener('input', calculateTotalDegree);
    degreeWithLanguageInput.addEventListener('input', calculateTotalDegree);








    //////////////////////////////
    function closeMessage(button) {
        const message = button.closest('.error-message, .success-message');
        message.classList.add('hide');
        setTimeout(() => {
            message.remove();
        }, 300);
    }

    const messages = document.querySelectorAll('.error-message, .success-message');
    messages.forEach(msg => {
        setTimeout(() => {
            msg.classList.add('hide');
            setTimeout(() => {
                msg.remove();
            }, 300);
        }, 50000); // auto-dismiss after 50 seconds
    });

});
