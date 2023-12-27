function deleteSelectedItems() {
    var selectedIds = [];
    var checkboxes = document.getElementsByClassName('checkbox');
    var atLeastOneChecked = false;

    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].checked) {
            atLeastOneChecked = true;
            selectedIds.push(parseInt(checkboxes[i].value));
        }
    }

    if (!atLeastOneChecked) {
        Swal.fire({
            icon: 'warning',
            title: 'Cảnh báo!',
            text: 'Vui lòng chọn ít nhất một mục để xoá!'
        });
        return;
    }


    var currentUrl = window.location.href;
    var delimiter = '/Admin/'; // Chuỗi ký tự mà bạn muốn sử dụng làm điểm chia

    // Sử dụng hàm indexOf để tìm vị trí của chuỗi delimiter trong đường dẫn URL
    var endIndex = currentUrl.indexOf(delimiter) + delimiter.length;

    // Sử dụng hàm indexOf để tìm vị trí của dấu / tiếp theo sau chuỗi delimiter
    var nextSlashIndex = currentUrl.indexOf('/', endIndex);

    // Nếu có dấu / tiếp theo sau chuỗi delimiter, cắt đến vị trí của dấu /, ngược lại, lấy toàn bộ chuỗi sau delimiter
    var baseUrl = (nextSlashIndex !== -1) ? currentUrl.substring(0, nextSlashIndex) : currentUrl.substring(0, endIndex);
    var ajaxUrl = baseUrl + '/DeleteMultiple';
    console.log(ajaxUrl);


    $.ajax({
        url: ajaxUrl,
        type: 'POST',
        data: { ids: selectedIds },
        traditional: true,
        success: function (data) {
            Swal.fire({
                icon: 'success',
                title: 'Thành công!',
                text: 'Các mục đã được xoá thành công!',
                timer: 2000,
                showConfirmButton: false
            }).then(() => {
                location.reload();
            });
        },
        error: function (error) {
            Swal.fire({
                icon: 'error',
                title: 'Lỗi!',
                text: 'Đã xảy ra lỗi hoặc thương hiệu đã được liên kết sảp phẩm!'
            });
        }
    });
}
