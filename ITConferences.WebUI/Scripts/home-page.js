$m = jQuery.noConflict();

//pass name filter to filter conferences
$m("#homeButton").click(function () {

    $m("#loading").show();
    $("#conferences").hide();

    $m.ajax({
        url: '/Conferences/Index/',
        type: "POST",
        dataType: "HTML",
        data: { nameFilter: $m("#name").val() },
        success: function (data) {
            $m("#conferences").html(data);

        },
        complete: function () {
            $m("#loading").hide();
            $m("#conferences").show();
        }
    });


});