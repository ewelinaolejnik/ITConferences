$m = jQuery.noConflict();


$m(document).ready(function () {

    $m("#TargetCountryId").change(function () {
        $("#conferences").hide();
        $m("#loading").show();
        
        $m.ajax({
            url: 'GetSelectedCities',
            type: "POST",
            dataType: "JSON",
            data: { countryID: $m(this).val() },
            success: function (data) {
                $m("#TargetCityId").empty(); // clear before appending new list 
                var items = '<option>Select city</option>';
                $m.each(data, function (i, city) {
                    items += "<option value='" + city.Value + "'>" + city.Text + "</option>";
                });
                $m('#TargetCityId').html(items);
                $m("#loading").hide();
                $m("#conferences").show();
            },
            error: function (ex) {
                $m("#loading").hide();
                $m("#conferences").show();
                alert('Failed to retreive cities. ' + ex);
            }
        });

        //for create cascading drop down list with cities
        var country = $m('#TargetCountryId');
        $m.ajax({
            url: 'Conferences/GetSelectedCities',
            type: "POST",
            dataType: "JSON",
            data: { country: country },
            success: function (cities) {
                $m("#City").html(""); // clear before appending new list 
                $m.each(cities, function (i, city) {
                    $m("#City").append(
                        $m('<option></option>').val(city.CityID).html(city.Name));
                });
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
        var dataConf = {
            Name: $m("#Name").val(), TargetCityId: $m("#TargetCityId").val(),
            Date: $m("#Date").val(), Url: $m("#Url").val(),
            IsPaid: $m("#IsPaid").val(), TargetCountryId: $m("#TargetCountryId").val(),
            tags: $m("#tags").val(), image: $m("#image").get(0).files[0]
        }

        var formData = new FormData();
        formData.append("image", $m("#image").prop('files')[0]);
        formData.append("tags", $m("#tags").val());
        formData.append("Name", $m("#Name").val());
        formData.append("TargetCityId", $m("#TargetCityId").val());
        formData.append("Date", $m("#Date").val());
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


    //$m("#img").change(function () {
    //    var dataF = new FormData();
    //    var files = $("#img").get(0).files;
    //    if (files.length > 0) {
    //        dataF.append("HelpSectionImages", files[0]);
    //    }
    //    $m.ajax({
    //        url: '/Conferences/UploadImage/',
    //        type: "POST",
    //        dataType: "HTML",
    //        processData: false,
    //        contentType: false,
    //        data: dataF,
    //        success: function (data) {
    //           // $m("#imageId").html(data);
    //        }
    //    });
    //});



});

