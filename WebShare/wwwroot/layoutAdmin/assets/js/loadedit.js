
//  ảnh
var input = document.getElementById("imageUpload");
var preview = document.getElementById("previewImage");
var oldImage = document.getElementById("oldImage");

input.addEventListener("change", function () {
    var file = this.files[0];

    if (file) {
        var reader = new FileReader();

        reader.addEventListener("load", function () {
            if (oldImage) {
                oldImage.style.display = "none"; // ẩn ảnh cũ
            }
            preview.setAttribute("src", this.result);
            preview.style.display = "block"; // hiển thị ảnh mới
        });

        reader.readAsDataURL(file);
    }
});

// ảnh 1
var input1 = document.getElementById("imageUpload1");
var preview1 = document.getElementById("previewImage1");
var oldImage1 = document.getElementById("oldImage1");

input1.addEventListener("change", function () {
    var file = this.files[0];

    if (file) {
        var reader = new FileReader();

        reader.addEventListener("load", function () {
            if (oldImage1) {
                oldImage1.style.display = "none"; // ẩn ảnh cũ
            }
            preview1.setAttribute("src", this.result);
            preview1.style.display = "block"; // hiển thị ảnh mới
        });

        reader.readAsDataURL(file);
    }
});

// ảnh 2
var input2 = document.getElementById("imageUpload2");
var preview2 = document.getElementById("previewImage2");
var oldImage2 = document.getElementById("oldImage2");

input2.addEventListener("change", function () {
    var file = this.files[0];

    if (file) {
        var reader = new FileReader();

        reader.addEventListener("load", function () {
            if (oldImage2) {
                oldImage2.style.display = "none"; // ẩn ảnh cũ
            }
            preview2.setAttribute("src", this.result);
            preview2.style.display = "block"; // hiển thị ảnh mới
        });

        reader.readAsDataURL(file);
    }
});

// ảnh 3
var input3 = document.getElementById("imageUpload3");
var preview3 = document.getElementById("previewImage3");
var oldImage3 = document.getElementById("oldImage3");

input3.addEventListener("change", function () {
    var file = this.files[0];

    if (file) {
        var reader = new FileReader();

        reader.addEventListener("load", function () {
            if (oldImage3) {
                oldImage3.style.display = "none"; // ẩn ảnh cũ
            }
            preview3.setAttribute("src", this.result);
            preview3.style.display = "block"; // hiển thị ảnh mới
        });

        reader.readAsDataURL(file);
    }
});
