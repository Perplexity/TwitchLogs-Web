var lastStart = "";
var lastEnd = "";
$(document).ready(function ($) {
    'use strict';

    $.datetimepicker.setDateFormatter('moment');

    $("#dtpStart").datetimepicker({
        format: "DD/MM/YYYY HH:mm",
        value: moment().startOf('day').format("DD/MM/YYYY HH:mm")
    });

    $("#dtpEnd").datetimepicker({
        format: "DD/MM/YYYY HH:mm:ss",
        value: moment()
    });

    renderLogTables();

    lastStart = $("#dtpStart").val();
    lastEnd = $("#dtpEnd").val();

    $("#dtpStart,#dtpEnd").on("change", function () {
        const start = $("#dtpStart").val();
        const end = $("#dtpEnd").val();
        if (start === lastStart && end === lastEnd) {
            return;
        }
        lastStart = start;
        lastEnd = end;
        $(".log-table").DataTable().destroy();
        renderLogTables();
    });

    $('.collapse').on('shown.bs.collapse', function () {
        $(this).parent().find(".fa-angle-down").removeClass("fa-angle-down").addClass("fa-angle-up");
    }).on('hidden.bs.collapse', function () {
        $(this).parent().find(".fa-angle-up").removeClass("fa-angle-up").addClass("fa-angle-down");
    });
});

function renderLogTables() {
    $.each($(".log-table"), function (i, element) {
        element = $(element);
        const channel = element.attr("data-channel");
        const sender = element.attr("data-sender");
        const timezone = element.attr("data-timezone");
        renderLogsTable(channel, sender, timezone, element);
    });
}

function renderLogsTable(channel, sender, timezone, element) {
    const from = moment($("#dtpStart").val(), "DD/MM/YYYY HH:mm").tz(timezone).valueOf();
    const to = moment($("#dtpEnd").val(), "DD/MM/YYYY HH:mm:ss").tz(timezone).valueOf();
    element.DataTable({
        ajax: `/Logs/Get/${channel}/${sender}?from=${from}&to=${to}`,
        deferRender: true,
        columns: [
            { data: "timestamp", type: "date" },
            { data: "sender" },
            { data: "message" }
        ],
        columnDefs: [
            {
                targets: 0, responsivePriority: -1, render: function (data) {
                    return moment(data).tz(timezone).format('DD/MM/YYYY HH:mm:ss');
                },
                className: "all"
            },
            {
                targets: 1, className: "all"
            },
            {
                targets: 2, className: "all"
            }],
        rowCallback: (row, data, index) => {
            if (data.type > 0) {
                $(row).css("background-color", "rgba(0, 0 ,0 , .05)");
                $(row).html(`<td>${moment(data.timestamp).tz(timezone).format('DD/MM/YYYY HH:mm:ss')}</td><td class='text-center text-danger' colspan='2'>${data.message}</td>`);
            }
        },
        order: [[0, "desc"]],
        lengthMenu: [[25, 100, 200, 500, 1000], [25, 100, 200, 500, 1000]],
        responsive: {
            details: false
        }
    });
}