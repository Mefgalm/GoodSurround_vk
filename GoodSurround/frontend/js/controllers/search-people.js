angular.module('GoodSurround').controller('PeopleSearchController', [ '$uibModalInstance', '$scope', 'Users',
function ($uibModalInstance, $scope, Users) {
    $scope.ok = function () {
        $uibModalInstance.close($scope.selectedUsers);
    };

    $scope.searchQuery = '';
    $scope.displayResultsThreshold = 3;
    $scope.searchResults = [];
    $scope.selectedUsers = [];

    // this'd better be ng-change handler
    $scope.$watch('searchQuery', function (newValue, oldValue) {
        console.log(newValue, oldValue);
        Users.find(newValue).then(function (results) {
            // todo: implement infinite scroll
            $scope.searchResults = results.map(function (user) {
                user.isSelected = false;
                return user;
            });
            $scope.selectedUsers = _.intersection($scope.selectedUsers, $scope.searchResults)
                .map(function (user) {
                    user.isSelected = true;
                    return user;
                });
            // to clear users when search results are not displayed
            if ($scope.searchQuery === '') {
                $scope.selectedUsers = [];
            }
            $scope.$apply();
        });
    });

    $scope.toggleSelection = function (user) {
        if (_.contains($scope.selectedUsers, user)) {
            $scope.selectedUsers = _.without($scope.selectedUsers, user);
        } else {
            $scope.selectedUsers.push(user);
        }
        user.isSelected = !user.isSelected;
    };
}]);
