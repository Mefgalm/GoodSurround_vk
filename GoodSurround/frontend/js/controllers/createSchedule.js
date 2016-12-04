angular.module('GoodSurround').controller('CreateScheduleController', ['$scope', '$uibModal',
function ($scope, $uibModal) {

    $scope.openSearchModal = function () {
        var modalInstance = $uibModal.open({
            animation: true,
            templateUrl: 'search-users.html',
            controller: 'SearchUserController'
        });

        modalInstance.result.then(function success(selectedUsers) {
            $scope.users = $scope.users.concat(selectedUsers);
        }, function failure() {
            console.log('Modal dismissed at: ' + new Date());
        });
    };
}]);
