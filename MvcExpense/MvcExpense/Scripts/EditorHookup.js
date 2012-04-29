/// <reference path="jquery-1.4.4.js" />
/// <reference path="jquery-ui.js" />

$(document).ready(function () {
    $(".date").datepicker({
        dateFormat: "dd M yy D", // todo make system parameter
        onSelect: function (dateText, inst) {
            var displayElementId = $(this).attr("id");
            var valueElementId = displayElementId.split("_")[0];
            var date = $(this).datepicker("getDate");
            var dateString = date.toDateString();
            $("#" + valueElementId).val(dateString);
        }
    });

    //$('.date').datepicker({ dateFormat: "dd M yy D" });
});
