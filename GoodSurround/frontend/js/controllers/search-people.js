angular.module('GoodSurround').controller('PeopleSearchController', [ '$uibModalInstance', '$scope',
function ($uibModalInstance, $scope) {
    $scope.ok = function () {
        $uibModalInstance.close('ok');
    };

    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
}]);
