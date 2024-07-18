$(document).ready(function () {



    ClassicEditor.create(document.querySelector('#js-description'))

       
    // Thêm sự kiện click cho nút thêm mục
    $(document).on('click', '#colors-container .add-color', function () {
        var html = `
                    <div class="row mb-3">
                        <div class="col">
                                    <input type="color" class="form-control" name="Colors" />
                        </div>
                        <div class="col-4">
                            <button type="button" class="btn rounded-pill btn-icon btn-primary add-color"><i class="bx bx-plus me-0 bx-xs"></i></button>
                                                <button type="button" class="btn rounded-pill btn-icon btn-danger delete-item"><i class="bx bx-trash me-0"></i></button>
                        </div>
                    </div>
                `;
        $('#colors-container').append(html);
    });

    // $(document).on('click', '#images-container .add-image', function () {
    //     var html = `
    //         <div class="row mb-3">
    //             <div class="col-8">
    //                          <div class="d-flex align-items-start align-items-sm-center gap-4">
    //                                                 <img src="../assets/img/avatars/1.png" alt="user-avatar" class="d-block rounded" height="100" width="100">
    //                                                 <div class="button-wrapper">
    //                                                     <label for="upload" class="btn btn-primary me-2 mb-4" tabindex="0">
    //                                                         <span class="d-none d-sm-block">Upload new photo</span>
    //                                                         <i class="bx bx-upload d-block d-sm-none"></i>
    //                                                         <input type="file" name="Images" class="account-file-input" hidden="" accept="image/png, image/jpeg">
    //                                                     </label>
    //                                                     <button type="button" class="btn btn-outline-secondary account-image-reset mb-4">
    //                                                         <i class="bx bx-reset d-block d-sm-none"></i>
    //                                                         <span class="d-none d-sm-block">Reset</span>
    //                                                     </button>

    //                                                     <p class="text-muted mb-0">Allowed JPG, GIF or PNG. Max size of 800K</p>
    //                                                 </div>
    //                                             </div>
    //             </div>
    //                      <div class="col-4">
    //                                             <button type="button" class="btn rounded-pill btn-icon btn-primary add-image"><i class='bx bx-add-to-queue'></i></button>
    //                                             <button type="button" class="btn rounded-pill btn-icon btn-danger delete-item"><i class='bx bx-message-square-x'></i></button>
    //                                         </div>
    //         </div>
    //     `;
    //     $('#images-container').append(html);
    // });

    $(document).on('click', '#sizes-container .add-size', function () {
        var html = `
                    <div class="row mb-3">
                        <div class="col-8">
                            <input type="text" class="form-control" name="Sizes" />
                        </div>
         <div class="col-4">
                                                  <button type="button" class="btn rounded-pill btn-icon btn-primary add-size"><i class="bx bx-plus me-0 bx-xs"></i></button>
                                                <button type="button" class="btn rounded-pill btn-icon btn-danger delete-item"><i class="bx bx-trash me-0"></i></button>
                                                </div>
                    </div>
                `;
        $('#sizes-container').append(html);
    });

    // Hàm xóa mục chung
    function deleteItemHandler() {
        var $parentContainer = $(this).closest('.container-list'); // Thay `.container` bằng class của container chứa nút xóa

        // Kiểm tra xem có nhiều hơn một phần tử trong container không
        if ($parentContainer.find('.delete-item').length > 1) {
            // Xóa phần tử chỉ định
            $(this).parent().parent().remove();
        } else {
            // Nếu chỉ có một phần tử duy nhất, không thực hiện xóa
            console.log('Cannot delete the only item');
            // Thêm hành động hoặc thông báo phù hợp tại đây
        }
    }


   
    // Hàm xóa mục chung
    function deleteImageHandler() {
        
        var $imageWrapper = $(this).closest('.uploaded-image');
        var filename = $imageWrapper.find('img').data('filename'); // Sử dụng data để lấy filename
        var fileInput = $(this).closest('.image-row').find('.account-file-input')[0]; // Lấy phần tử input file



        // Xóa ảnh từ danh sách đã tải lên
        $imageWrapper.remove();

        // Tạo đối tượng DataTransfer để thao tác với danh sách tệp
        var dataTransfer = new DataTransfer();

        // Lặp qua danh sách tệp tin và loại bỏ tệp tin tương ứng
        var files = fileInput.files;
        for (var j = 0; j < files.length; j++) {          
            if (files[j].name !== filename) {
                dataTransfer.items.add(files[j]);
            }
        }

        // Gán lại danh sách tệp cho input file
        fileInput.files = dataTransfer.files;
    }



    // Thêm sự kiện click cho nút xóa mục trong các container khác nhau
    $(document).on('click', '#colors-container .delete-item', deleteItemHandler);
    $(document).on('click', '#sizes-container .delete-item', deleteItemHandler);
    /*$(document).on('click', '.uploaded-images .delete-image', deleteImageHandler);*/

    // Xử lý khi người dùng thay đổi tệp tải lên
    $(document).on('change', '.account-file-input', function () {
        var $uploadedImages = $(this).closest('.image-row').find('.uploaded-images');
        $uploadedImages.empty(); // Xóa ảnh đã tải lên trước đó

        var filesArray = Array.from(this.files);
        if (filesArray.length > 0) {
            filesArray.forEach((file) => {
                var reader = new FileReader();
                reader.onload = function (e) {
                    // Tạo imgWrapper, img và deleteBtn
                    var imgWrapper = $('<div>').addClass('row mb-3 uploaded-image');
                    var imgCol = $('<div>').addClass('col-8');

                    var img = $('<img>').attr({
                        src: e.target.result,
                        alt: 'uploaded-image',
                        class: 'd-block rounded',
                        height: "100",
                        width: "100",
                        style: 'object-fit: cover',
                        'data-filename': file.name // Thêm thuộc tính data-filename
                    });

                    var deleteCol = $('<div>').addClass('col-4');
                    var deleteBtn = $('<button>').attr({
                        type: 'button',
                        class: 'btn btn-danger delete-image'
                    }).html('<i class="bx bx-trash"></i>');

                    // Bắt sự kiện click vào nút xóa
                    deleteBtn.on('click', function () {
                        var $imageWrapper = $(this).closest('.uploaded-image');
                        var filename = $imageWrapper.find('img').data('filename'); // Sử dụng data để lấy filename
                        var fileInput = $(this).closest('.image-row').find('.account-file-input')[0]; // Lấy phần tử input file



                        // Xóa ảnh từ danh sách đã tải lên
                        $imageWrapper.remove();

                        // Tạo đối tượng DataTransfer để thao tác với danh sách tệp
                        var dataTransfer = new DataTransfer();

                        // Lặp qua danh sách tệp tin và loại bỏ tệp tin tương ứng                       
                        var files = Array.from(fileInput.files);
                        files.forEach((file) => file.name !== filename && dataTransfer.items.add(file))                      
                        // Gán lại danh sách tệp cho input file
                        fileInput.files = dataTransfer.files;
                    });

                    // Thêm img và deleteBtn vào imgCol và deleteCol, sau đó thêm imgCol và deleteCol vào imgWrapper và đưa imgWrapper vào $uploadedImages
                    imgCol.append(img);
                    deleteCol.append(deleteBtn);
                    imgWrapper.append(imgCol);
                    imgWrapper.append(deleteCol);
                    $uploadedImages.append(imgWrapper);
                };
                reader.readAsDataURL(file);
            })
        }
    });

    // Xử lý khi người dùng nhấn nút Reset
    $(document).on('click', '.account-image-reset', function () {
        resetFileInput(); // Gọi hàm để reset input file

        var $uploadedImages = $(this).closest('.image-row').find('.uploaded-images');
        $uploadedImages.empty(); // Xóa các ảnh đã tải lên
    });

    // Hàm reset input file
    function resetFileInput() {
        var $fileInput = $('.account-file-input');
        $fileInput.val(''); // Đặt lại giá trị của input file
    }
});