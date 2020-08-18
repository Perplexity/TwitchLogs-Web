$(document).ready(function ($) {
    'use strict';
    $("#stalkForm").on("submit", e => {
        e.preventDefault();
        const user = $("#stalkUser").val();
        location.href = `/Logs/Stalk/${user}`;
    });
    $("#addChannel").on("click", function () {
        Swal.fire({
            title: "Enter channel name",
            input: "text",
            showCancelButton: true,
            confirmButtonText: "Add",
            confirmButtonColor: "#2ec551",
            showLoaderOnConfirm: true,
            preConfirm: (channel) => {
                return $.get(`/Channel/Add/${channel}`).then(response => {
                    if (!response.success) {
                        throw new Error(response.message);
                    }
                    return response;
                }).catch(error => {
                    Swal.showValidationMessage(error);
                });
            }
        }).then((result) => {
            const response = result.value;
            Swal.fire({
                icon: "success",
                title: "Success",
                text: response.message
            }).then(() => {
                location.reload();
            });
        });
    });
    $(".delete-channel").on("click", function () {
        const channelId = $(this).attr("data-channel");
        Swal.fire({
            title: "Are you sure?",
            text: "If you remove this channel, you won't be able to view the logs and your credit will not be refunded.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#ef172c",
            confirmButtonText: "Yes, remove this channel",
            cancelButtonText: "Nevermind",
            showLoaderOnConfirm: true,
            preConfirm: () => {
                return $.get(`/Channel/Remove/${channelId}`).then(response => {
                    if (!response.success) {
                        throw new Error(response.message);
                    }
                    return response;
                }).catch(error => {
                    Swal.showValidationMessage(error);
                });;
            }
        }).then((result) => {
            const response = result.value;
            Swal.fire({
                icon: "success",
                title: "Success",
                text: response.message
            }).then(() => {
                location.reload();
            });
        });
    });
});