angular.module('GoodSurround').controller('ScheduleController', ['$scope', '$uibModal', 'Users',
    function ($scope, $uibModal, Users) {
        $scope.displayUsers = false;

        $scope.toggleUsersPanel = function () {
            $scope.displayUsers = !$scope.displayUsers;
        };

        Users.saved.getAll().then(function (users) {
            $scope.users = users;
        });

        $scope.openSearchModal = function () {
            var modalInstance = $uibModal.open({
                animation: true,
                templateUrl: 'search-people.html',
                controller: 'PeopleSearchController'
            });

            modalInstance.result.then(function success (selectedUsers) {
                $scope.users = $scope.users.concat(selectedUsers);
            }, function failure () {
                console.log('Modal dismissed at: ' + new Date());
            });
        };
    }]);
