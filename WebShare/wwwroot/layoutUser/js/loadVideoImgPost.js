function handleFiles(files, containerId) {
	const container = document.getElementById(containerId);

	// Xóa nội dung hiện tại của container
	container.innerHTML = '';

	// Lặp qua từng file và hiển thị ảnh hoặc video
	for (const file of files) {
		const fileType = file.type.split('/')[0]; // Lấy loại tệp (image hoặc video)

		const mediaElement = document.createElement(fileType === 'image' ? 'img' : 'video');
		mediaElement.setAttribute('controls', '');

		const objectURL = URL.createObjectURL(file);
		mediaElement.src = objectURL;

		container.appendChild(mediaElement);

	}
	// Kiểm tra nếu container có con, nếu có thì thêm margin-top
	if (container.hasChildNodes()) {
		container.style.marginTop = '10px';
	} else {
		container.style.marginTop = '0';
	}
}
