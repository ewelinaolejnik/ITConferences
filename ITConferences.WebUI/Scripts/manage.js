$m = jQuery.noConflict();

$m(document).ready(function () {
    
    $m("#image").change(function () {
        var formData = new FormData();
        formData.append("image", $m("#image").prop('files')[0]);
        $m.ajax({
            url: '/Manage/SetImage/',
            type: "POST",
            dataType: "HTML",
            processData: false,
            contentType: false,
            data: formData,
            success: function (data) {
                $m("#main_nav").empty();
                $m("#main_nav").html(data);

            }
        });

    });

});
