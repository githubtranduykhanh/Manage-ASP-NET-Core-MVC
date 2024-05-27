



const managerUser = (() => {
    let tableUsers

    const managerUserStore = {
        init() {
            tableUsers = new DataTable('#js-table-users', {
                responsive: true,
                columnDefs: [{
                    targets: 4, // index của cột "Address"
                    render: function (data, type, row) {
                        if (type === 'display') {
                            return data.length > 25 ? data.substr(0, 25) + '...' : data;
                        }
                        return data;
                    }
                }, {
                    targets: 2, // index của cột "Email"
                    render: function (data, type, row) {
                        if (type === 'display') {
                            return data.length > 20 ? data.substr(0, 20) + '...' : data;
                        }
                        return data;
                    }   
                }]
            });
        }
    }
    return managerUserStore
})()


export default managerUser