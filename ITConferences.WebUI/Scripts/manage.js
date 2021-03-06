﻿$m = jQuery.noConflict();

$m(document).ready(function() {

    $m("#image").change(function() {
        var formData = new FormData();
        formData.append("image", $m("#image").prop("files")[0]);
        $m.ajax({
            url: "/Manage/SetImage/",
            type: "POST",
            dataType: "HTML",
            processData: false,
            contentType: false,
            data: formData,
            success: function(data) {
                $m("#main_nav").empty();
                $m("#main_nav").html(data);

            }
        });

    });


    //pass parameters to change conference
    $m("#manageButton").click(function() {
        var formData = new FormData();
        formData.append("imageManage", $m("#imageManage").prop("files")[0]);
        formData.append("tags", $m("#tags").val());
        formData.append("speakers", $m("#speakers").val());
        formData.append("Name", $m("#Name").val());
        formData.append("TargetCityId", $m("#TargetCityId").val());
        formData.append("ConferenceID", $m("#ConferenceID").val());
        formData.append("StartDate", $m("#StartDate").val());
        formData.append("EndDate", $m("#EndDate").val());
        formData.append("Url", $m("#Url").val());
        formData.append("IsPaid", $m("#IsPaid").prop("checked"));
        formData.append("TargetCountryId", $m("#TargetCountryId").val());

        $m.ajax({
            url: "/Conferences/Manage/",
            type: "POST",
            dataType: "HTML",
            processData: false,
            contentType: false,
            data: formData,
            success: function(data) {
                $m("#manage").html(data);
            }
        });
    });

    //create multiselect tags
    $m("#tags").multiselect({
        noneSelectedText: "Select tags",
        selectedList: 4
    }).multiselectfilter();

    $m("#speakers").multiselect({
        noneSelectedText: "Select speakers",
        selectedList: 4
    }).multiselectfilter();

});