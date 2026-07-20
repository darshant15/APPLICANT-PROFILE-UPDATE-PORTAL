const fileInput = document.getElementById('Document');
const dropzone = document.getElementById('dropzone');

if (fileInput && dropzone) {
    dropzone.addEventListener('click', () => fileInput.click());
    fileInput.addEventListener('change', () => {
        if (fileInput.files.length > 0) {
            dropzone.classList.add('filled');
            dropzone.textContent = `✓ ${fileInput.files[0].name} attached — click to replace`;
        }
    });
}
