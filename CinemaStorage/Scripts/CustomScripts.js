$(document).ready(function () {

    // datepicker initialization
    $('.datepicker').datepicker({
        format: 'yyyy-mm-dd',
        weekStart: 1
    });

    // image loader initialization
    $("#movie-poster-input").change(function () {
        var data = new FormData();
        var files = $("#movie-poster-input").get(0).files;
        if (files.length > 0) {
            data.append("Images", files[0]);
        }
        $.ajax({
            url: "/Movies/UploadImageAjax/",
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (response) {                
                alert('Изображение загружено!');
            },
            error: function (er) {
                alert('Ошибка при загрузке. Попробуйте еще раз.');
            }

        });
    });
});