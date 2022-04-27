$(document).ready(function () {
    $.ajax({
        type: "POST",
        url: 'DashBoardcounts',
        data: JSON.stringify({}),
        contentType: "application/json:charset=utf-8",
        dataType: "json",
        success: function (json) {
            //debugger
            var values = json.DashBoardcounts;
            var date = parseInt(values[0]);
            var closuredate = parseInt(values[1]);
            // Build the chart
            Highcharts.chart('container', {
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false,
                    type: 'pie'
                },
                title: {
                    text: 'Date Submission Count'
                },
                tooltip: {
                    pointFormat: '{series.name}: <b>{point.y} Submission</b>'
                },
                accessibility: {
                    point: {
                        //valueSuffix: '%'
                        valueSuffix: 'Submission'
                    }
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: false
                        },
                        showInLegend: true
                    }
                },
                series: [{
                    name: 'Count',
                    colorByPoint: true,
                    data: [{
                        name: '2022',
                        y: date,
                        sliced: true,
                        selected: true
                    }, {
                        name: '2021',
                        y: closuredate
                    }]
                }]
            });
        }
    })
});


