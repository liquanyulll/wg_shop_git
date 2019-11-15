app.controller('ProductListCtrl', function ($scope, $http, $rootScope, $stateParams, $state) {
	var init = function () {
		$scope.InfoList = [];
		$scope.queryVar = {};
		/* 配置分页基本参数 */
		$scope.paginationConf = {
			currentPage: 1,
			itemsPerPage: 20
		};
        $scope.paginationConf.perPageOptions = [20];
        $scope.queryVar.PName = $stateParams.pname;
	}
	init();

	$scope.Search = function () {
		$scope.queryVar.PageIndex = $scope.paginationConf.currentPage;
		$scope.queryVar.PageSize = $scope.paginationConf.itemsPerPage;
        $scope.queryVar.TypeId = $stateParams.tpid;
        $scope.queryVar.TypeAll = $stateParams.all;

		$http.post(GetUrl('api/Product/Search'), $scope.queryVar).then(function (resp) {
			var qRtn = resp.data;
			$scope.InfoList = qRtn.contentList
			if ($scope.InfoList != null && $scope.InfoList.length > 0) {
				$scope.paginationConf.totalItems = qRtn.totalItems;
			} else {
				$scope.paginationConf.totalItems = 1;
			}
		}).catch(function (resp) {
			console.log(resp);
		});
	};
	$scope.Search();

 //   $scope.$on("goHeadSearch", function (event, data) {
	//	$scope.Search();
	//});

	$scope.GoProduct = function (x) {
		var url = $state.href("index.productList.product", { id: x.productId });
		window.open(url, '_blank');
	}

	//$http.get(GetUrl('api/Account/CurrentUser')).then(function (res) {
	//    $scope.user = res.data;
	//}).catch(function (resp) {
	//    console.log(resp);
	//});

});
