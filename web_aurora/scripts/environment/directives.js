var directives = angular.module('directives', []);

directives.directive('datePicker', function () {
    return {
        restrict: 'AE',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            $(function () {
                element.datepicker({
                    changeMonth: true,
                    changeYear: true,
                    showButtonPanel: true,
                    showMonthAfterYear: true,
                    defaultDate: '+1d', //默认日期
                    minDate: '-1Y', //最小日期
                    maxDate: '2099-12-30', //最大日期
                    dateFormat: 'yy-mm-dd',
                    monthNamesShort: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
                    dayNamesMin: ['日', '一', '二', '三', '四', '五', '六'],
                    prevText: '上个月',
                    nextText: '下个月',
                    currentText: '今天',
                    closeText: '关闭',
                    onSelect: function (date) {
                        scope.$apply(function () {
                            ngModelCtrl.$setViewValue(date);
                        });
                    }
                });
            });
        }
    }
});
