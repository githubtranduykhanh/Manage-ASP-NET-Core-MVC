

    const exportButtonConfig = (type, iconClass, text, title) => {
        return {
            extend: type,
            text: `<i class="${iconClass}"></i>${text}`,
            className: "dropdown-item",
            title,
            filename: () => `${title} - ${Math.floor(Date.now() / 1000)}`,
            exportOptions: {
                columns: [0, 1],
                format: {
                    body: (data, row, column, node) => (column === 3 || column === 1) ?
                        ($(node).attr('data-original-value') || data) :
                        ($(node).find(':input').val() || data)
                }
            }
        };
    }

const renderTable = ({ tableID, columns, url, tableName, FnCreate, initComplete }) => {

    const table = $(tableID).DataTable({            
            processing: true,
            serverSide: true,
            responsive: true,
            dom: '<"card-header flex-column flex-md-row"<"head-label text-center"><"dt-action-buttons text-end pt-3 pt-md-0"B>><"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6 d-flex justify-content-center justify-content-md-end"f>>t<"row"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>', // B: Buttons, f: Filter input, r: Processing, t: Table, i: Table information, p: Pagination
            ajax: {
                url,
                type: "POST",  
                "datatype": "json"
            },
           
            columns,
            buttons: [
                {
                    extend: "collection",
                    className: "btn btn-label-primary dropdown-toggle me-2",
                    text: '<i class="bx bx-export me-sm-1"></i> <span class="d-none d-sm-inline-block">Export</span>',
                    buttons: [
                        exportButtonConfig("print", "bx bx-printer me-1", "Print", tableName),
                        exportButtonConfig("csv", "bx bx-file me-1", "Csv", tableName),
                        exportButtonConfig("excel", "bx bxs-file-export me-1", "Excel", tableName),
                        exportButtonConfig("pdf", "bx bxs-file-pdf me-1", "Pdf", tableName),
                        exportButtonConfig("copy", "bx bx-copy me-1", "Copy", tableName)
                    ]
                },
                {
                    text: '<i class="bx bx-plus me-sm-1"></i> <span class="d-none d-sm-inline-block">Add Claim</span>',
                    className: "create-new-role-claim btn btn-primary",
                    action: FnCreate
                }
            ],       
            language: {
                processing: `<div class="spinner-border spinner-border-lg text-primary" role="status">
                          <span class="visually-hidden">Loading...</span>
                        </div>`,
            },
            initComplete,      
        });
        const headLabel = document.querySelector('div.head-label')
        headLabel.innerHTML = `<h5 class="card-title mb-0">${tableName}</h5>`
      

        return table
}



const renderTableNoAjax = ({ tableID, tableName }) => {
    const table = $(tableID).DataTable({  
        responsive: true,
        dom: '<"card-header flex-column flex-md-row"<"head-label text-center"><"dt-action-buttons text-end pt-3 pt-md-0"B>><"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6 d-flex justify-content-center justify-content-md-end"f>>t<"row"<"col-sm-12 col-md-6"i><"col-sm-12 col-md-6"p>>', // B: Buttons, f: Filter input, r: Processing, t: Table, i: Table information, p: Pagination
        buttons: [
            {
                extend: "collection",
                className: "btn btn-label-primary dropdown-toggle me-2",
                text: '<i class="bx bx-export me-sm-1"></i> <span class="d-none d-sm-inline-block">Export</span>',
                buttons: [
                    exportButtonConfig("print", "bx bx-printer me-1", "Print", tableName),
                    exportButtonConfig("csv", "bx bx-file me-1", "Csv", tableName),
                    exportButtonConfig("excel", "bx bxs-file-export me-1", "Excel", tableName),
                    exportButtonConfig("pdf", "bx bxs-file-pdf me-1", "Pdf", tableName),
                    exportButtonConfig("copy", "bx bx-copy me-1", "Copy", tableName)
                ]
            },
            {
                text: '<i class="bx bx-plus me-sm-1"></i> <span class="d-none d-sm-inline-block">Add Claim</span>',
                className: "create-new-role-claim btn btn-primary",    
            }
        ],        
    });   
    const headLabel = document.querySelector(`${tableID}_wrapper`).querySelector('div.head-label')
    headLabel.innerHTML = `<h5 class="card-title mb-0">${tableName}</h5>`
  
    return table
}
