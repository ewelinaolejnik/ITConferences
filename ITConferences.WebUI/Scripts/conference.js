$m = jQuery.noConflict();


$m(document).ready(function () {

    $m("#TargetCountryId").change(function () {
        $m.ajax({
            url: 'GetSelectedCities',
            type: "POST",
            dataType: "JSON",
            data: { countryID: $m("#TargetCountryId").val() },
            success: function (data) {
                $m("#TargetCityId").empty(); // clear before appending new list 
                var items = '<option>Select city</option>';
                $m.each(data, function (i, city) {
                    items += "<option value='" + city.Value + "'>" + city.Text + "</option>";
                });
                $m('#TargetCityId').html(items);
            },
            error: function (ex) {
                alert('Failed to retreive cities. ' + ex);
            }
        });
        
    });

    //create multiselect tags
    $m("#tags").multiselect({
        noneSelectedText: "Select tags",
        selectedList: 4
    }).multiselectfilter();



    //token for safety
    var AddAntiForgeryToken = function (data) {
        data.__RequestVerificationToken = $('#__AjaxAntiForgeryForm input[name=__RequestVerificationToken]').val();
        return data;
    };

    //pass parameters to add conference
    $m("#submitButton").click(function () {
        var formData = new FormData();
        formData.append("imageCreate", $m("#imageCreate").prop('files')[0]);
        formData.append("tags", $m("#tags").val());
        formData.append("Name", $m("#Name").val());
        formData.append("TargetCityId", $m("#TargetCityId").val());
        formData.append("StartDate", $m("#StartDate").val());
        formData.append("EndDate", $m("#EndDate").val());
        formData.append("Url", $m("#Url").val());
        formData.append("IsPaid", $m('#IsPaid').prop('checked'));
        formData.append("TargetCountryId", $m("#TargetCountryId").val());
        if ($("#userId").is(':checked'))
            formData.append("userId", $m("#userId").val());

        $m.ajax({
            url: '/Conferences/Create/',
            type: "POST",
            dataType: "HTML",
            processData: false,
            contentType: false,
            data: formData,
            success: function (data) {
                $m("#conferences").html(data);
            }
        });
    });


    $m("#location").autocomplete({
        source: function (request, response) {
            $m.ajax({
                url: '/Conferences/GetLocations/',
                type: "POST",
                dataType: "JSON",
                data: { locationFilter: $m("#location").val() },
                success: function (data) {
                    response(data);
                },
                error: function (res) {
                    alert("Error" + res.responseText);
                }
            });
        }
    });


    var page = 0;

    //pass parameters to filter conferences
    $m("#submitButtonIndex").click(function () {
        updateConferences();
    });

    //pass parameters to filter conferences by click enter
    $('.submit').keypress(function (e) {
        if (e.which == 13) {
            updateConferences();
        }
    });

    function updateConferences() {
        $m("#loading").show();
        $("#conferences").hide();

        $m.ajax({
            url: '/Conferences/GetConferences/',
            type: "POST",
            dataType: "HTML",
            data: { selectedTagsIds: $m("#tags").val(), locationFilter: $m("#location").val(), nameFilter: $m("#name").val(), dateFilter: $m("#dateFilter").val(), filter: true },
            success: function (data) {
                $m("#conferences").empty();
                $m("#conferences").html(data);

            },
            complete: function () {
                $m("#loading").hide();
                $m("#conferences").show();
                page = 0;
            }
        });
    }


    //paging site by scrolling
    var lastScrollTop = 0;
    var inCallback = false;

    function loadConferences() {
        if (page > -1 && !inCallback) {
            inCallback = true;
            page++;
            $m.ajax({
                url: '/Conferences/GetConferences/',
                type: "POST",
                dataType: "HTML",
                data: { page: page, dateFilter: $m("#dateFilter").val(), selectedTagsIds: $m("#tags").val(), locationFilter: $m("#location").val(), nameFilter: $m("#name").val() },
                success: function (data) {
                    if (data != '') {
                        $m("#conferences").append(data);
                    } else {
                        page = -1;
                    }

                },
                complete: function () {
                    inCallback = false;
                }

            });
        }

    }


    $m(window).scroll(function () {
        var st = $(this).scrollTop();
        var docHeight = $m(document).height();
        var windowHeight = $m(window).height();
        if ((st <= docHeight - windowHeight) && (st > lastScrollTop)) {
            loadConferences();
        }
        lastScrollTop = st;
    });


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


    //pass parameters to change conference
    $m("#manageButton").click(function () {
        var formData = new FormData();
        formData.append("image", $m("#image").prop('files')[0]);
        formData.append("tags", $m("#tags").val());
        formData.append("Name", $m("#Name").val());
        formData.append("TargetCityId", $m("#TargetCityId").val());
        formData.append("StartDate", $m("#StartDate").val());
        formData.append("EndDate", $m("#EndDate").val());
        formData.append("Url", $m("#Url").val());
        formData.append("IsPaid", $m('#IsPaid').prop('checked'));
        formData.append("TargetCountryId", $m("#TargetCountryId").val());
        if ($("#userId").is(':checked'))
            formData.append("userId", $m("#userId").val());

        $m.ajax({
            url: '/Conferences/Manage/',
            type: "POST",
            dataType: "HTML",
            processData: false,
            contentType: false,
            data: formData,
            success: function (data) {
                $m("#conferences").html(data);
            }
        });
    });

});

