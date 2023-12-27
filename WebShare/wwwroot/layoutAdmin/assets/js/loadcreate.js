var input = document.getElementById("imageUpload");
var preview = document.getElementById("previewImage");

input.addEventListener("change", function () {
    var file = this.files[0];

    if (file) {
        var reader = new FileReader();

        reader.addEventListener("load", function () {
            preview.setAttribute("src", this.result);
            preview.style.display = "block";
        });

        reader.readAsDataURL(file);
    }
});

var input1 = document.getElementById("imageUpload1");
var preview1 = document.getElementById("previewImage1");

input1.addEventListener("change", function () {
    var file = this.files[0];

    if (file) {
        var reader = new FileReader();

        reader.addEventListener("load", function () {
            preview1.setAttribute("src", this.result);
            preview1.style.display = "block";
        });

        reader.readAsDataURL(file);
    }
});

var input2 = document.getElementById("imageUpload2");
var preview2 = document.getElementById("previewImage2");

input2.addEventListener("change", function () {
    var file = this.files[0];

    if (file) {
        var reader = new FileReader();

        reader.addEventListener("load", function () {
            preview2.setAttribute("src", this.result);
            preview2.style.display = "block";
        });

        reader.readAsDataURL(file);
    }
});

var input3 = document.getElementById("imageUpload3");
var preview3 = document.getElementById("previewImage3");

input3.addEventListener("change", function () {
    var file = this.files[0];

    if (file) {
        var reader = new FileReader();

        reader.addEventListener("load", function () {
            preview3.setAttribute("src", this.result);
            preview3.style.display = "block";
        });

        reader.readAsDataURL(file);
    }
});