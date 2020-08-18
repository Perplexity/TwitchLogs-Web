$(document).ready(function ($) {
    'use strict';
    $("#updateTimezone").on("click", () => {
        $("#updateTimezone").prop("disabled", true);
        const timezoneId = $("#timezone option:selected").val();
        var data = {
            timezone: timezoneId
        }
        $.postJSON("/User/Update", data, (response) => {
            $("#updateTimezone").prop("disabled", false);
            toastr.options = {
                "closeButton": false,
                "debug": false,
                "newestOnTop": false,
                "progressBar": false,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
            toastr["success"]("Are you the six fingered man?")
        });
    });
});