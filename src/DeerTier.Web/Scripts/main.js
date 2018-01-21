$(document).ready(function () {
    $(".defaultTable").DataTable(
    {
        "paging": false,
        "info": false,
        "searching": false,
        "stripeClasses": [],
        "columnDefs": [
            { "targets": "nosort", "orderable": false },
            { "targets": ["dateSubmittedColumn", "escapeGameTimeColumn"], "orderSequence": ["desc", "asc"] }
        ]
    });


    if ($('#scoreDeletionLog').length) {
        $('#scoreDeletionLog').DataTable();
    }

    $('.navSectionExpander').on('click', function (e) {
        var $expander = $(e.target);
        var navSection = $expander.siblings('.navSectionContent')[0];
        var $navSection = $(navSection);
        NavSlider.slideToggle($expander, $navSection);
    });

    $("#hideRecordsWithoutVideo").on("change", function () {
        var url = location.pathname;
        if ($(this).is(":checked")) {
            url += "?hideRecordsWithoutVideo=1";
        }
        location.href = url;
    });
});

var NavSlider = {
    isToggling: false,

    slideToggle: function ($expander, $navSection) {
        if (!$navSection) {
            return;
        }

        if (!this.isToggling) {
            this.isToggling = true;

            this.flipExpanderArrow($expander);

            $navSection.slideToggle(200, function () {
                NavSlider.isToggling = false;
            });
        }
    },

    flipExpanderArrow: function ($expander) {
        if (!$expander) {
            return;
        }

        if($expander.hasClass("navSectionExpander_Collapsed")) {
            $expander.removeClass("navSectionExpander_Collapsed");
            $expander.addClass("navSectionExpander_Expanded");
        }
        else if($expander.hasClass("navSectionExpander_Expanded")) {
            $expander.removeClass("navSectionExpander_Expanded");
            $expander.addClass("navSectionExpander_Collapsed");
        }
    }
};