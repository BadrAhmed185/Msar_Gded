document.addEventListener('DOMContentLoaded', function () {


        const allowedExtensions = ['jpg', 'jpeg', 'png', 'gif', 'bmp', 'webp'];

    // Select all file inputs
    const fileInputs = document.querySelectorAll('input[type="file"]');

    fileInputs.forEach(function (input) {
        input.addEventListener('change', function () {
            const file = this.files[0];

            if (file) {
                const fileExtension = file.name.split('.').pop().toLowerCase();

                if (!allowedExtensions.includes(fileExtension)) {
                    alert("إمتداد الصورة لازم يكون من الإمتدادات دي ['jpg', 'jpeg', 'png', 'gif', 'bmp', 'webp'] فقط.");
                    this.value = ''; // Clear the file
                }
            }
        });
    });

    });

