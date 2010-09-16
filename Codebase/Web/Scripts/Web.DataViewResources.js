/// <reference name="MicrosoftAjax.debug.js" />
Type.registerNamespace('Web');
if (!Web) Web = {}

Web.DataViewResources = {}

Web.DataViewResources.Common = {
    WaitHtml: '<div class="Wait"></div>'
}

Web.DataViewResources.Pager = {
    ItemsPerPage: 'Items per page: ',
    PageSizes: [10, 15, 20, 25],
    ShowingItems: 'Showing <b>{0}</b>-<b>{1}</b> of <b>{2}</b> items',
    SelectionInfo: ' (<b>{0}</b> selected)',
    Refresh: 'Refresh',
    Next: 'Next »',
    Previous: '« Previous',
    Page: 'Page',
    PageButtonCount: 10
}

Web.DataViewResources.ActionBar = {
    View: 'View'
}

Web.DataViewResources.ModalPopup = {
    Close: 'Close',
    MaxWidth: 800,
    MaxHeight: 600,
    OkButton: 'OK',
    CancelButton: 'Cancel'
}

Web.DataViewResources.Menu = {
    SiteActions: 'Site Actions',
    SeeAlso: 'See Also',
    Summary: 'Summary',
    Tasks: 'Tasks'
}

Web.DataViewResources.HeaderFilter = {
    GenericSortAscending: 'Smallest on Top',
    GenericSortDescending: 'Largest on Top',
    StringSortAscending: 'Ascending',
    StringSortDescending: 'Descending',
    DateSortAscending: 'Earliest on Top',
    DateSortDescending: 'Latest on Top',
    EmptyValue: '(Empty)',
    BlankValue: '(Blank)',
    Loading: 'Loading...',
    ClearFilter: 'Clear Filter from {0}',
    SortBy: 'Sort by {0}',
    MaxSampleTextLen: 80,
    CustomFilterOption: 'Custom Filter...',
    CustomFilterPrompt: 'Show all rows in <span class="Highlight">{0}</span> where column <span class="Highlight">{1}</span> matches the following value(s). You may separate multiple values with comma and use &gt;, &lt;, &gt;=, or &lt;= operators placed before the value.',
    CustomFilterLimit: 5
}

Web.DataViewResources.InfoBar = {
    FilterApplied: 'A filter has been applied.',
    ValueIs: ' <span class="Highlight">{0}</span> ',
    Or: ' or ',
    And: ' and ',
    EqualTo: 'is equal to ',
    LessThan: 'is less than ',
    LessThanOrEqualTo: 'is less than or equal to ',
    GreaterThan: 'is greater than ',
    GreaterThanOrEqual: 'is greater than or equal to ',
    Like: 'is like ',
    StartsWith: 'starts with ',
    Empty: 'empty',
    QuickFind: ' Any field contains '
}

Web.DataViewResources.Lookup = {
    SelectToolTip: 'Select {0}',
    ClearToolTip: 'Clear {0}',
    NewToolTip: 'New {0}',
    SelectLink: '(select)',
    ShowActionBar: true,
    DetailsToolTip: 'View details for {0}',
    ShowDetailsInPopup: true,
    GenericNewToolTip: 'Create New'
}

Web.DataViewResources.Validator = {
    RequiredField: 'This field is required.',
    EnforceRequiredFieldsWithDefaultValue: false,
    NumberIsExpected: 'A number is expected.',
    BooleanIsExpected: 'A logical value is expected.',
    DateIsExpected: 'A date is expected.'
}

Web.DataViewResources.Data = {
    NullValue: '<span class="NA">n/a</span>',
    NullValueInForms: 'N/A',
    BooleanDefaultStyle: 'DropDownList',
    BooleanOptionalDefaultItems: [[null, 'N/A'], [false, 'No'], [true, 'Yes']],
    BooleanDefaultItems: [[false, 'No'], [true, 'Yes']],
    MaxReadOnlyStringLen: 600,
    NoRecords: 'No records found.',
    BlobHandler: '~/Blob.ashx',
    BlobDownloadLink: 'download',
    BlobDownloadLinkReadOnly: '<span style="color:gray;">download</span>',
    BlobDownloadHint: 'Click here to download the original file.',
    BatchUpdate: 'update',
    NoteEditLabel: 'edit',
    NoteDeleteLabel: 'delete',
    NoteDeleteConfirm: 'Delete?',
    UseLEV: 'Paste "{0}"'
}

Web.DataViewResources.Form = {
    ShowActionBar: true,
    ShowCalendarButton: true,
    RequiredFieldMarker: '<span class="Required">*</span>',
    RequiredFiledMarkerFootnote: '* - indicates a required field',
    SingleButtonRowFieldLimit: 7,
    GeneralTabText: 'General'
}

Web.DataViewResources.Grid = {
    InPlaceEditContextMenuEnabled: true,
    QuickFindText: 'Quick Find',
    QuickFindToolTip: 'Type to search the records and press Enter',
    Aggregates: {
        None: { FmtStr: '', ToolTip: '' },
        Sum: { FmtStr: 'Sum: {0}', ToolTip: 'Sum of {0}' },
        Count: { FmtStr: 'Count: {0}', ToolTip: 'Count of {0}' },
        Avg: { FmtStr: 'Avg: {0}', ToolTip: 'Average of {0}' },
        Max: { FmtStr: 'Max: {0}', ToolTip: 'Maximum of {0}' },
        Min: { FmtStr: 'Min: {0}', ToolTip: 'Minimum of {0}' }
    }
}

Web.DataViewResources.Views = {
    DefaultDescriptions: {
        '$DefaultGridViewDescription': 'This is a list of {0}.',
        '$DefaultEditViewDescription': 'Please review {0} information below. Click Edit to change this record, click Delete to delete the record, or click Cancel/Close to return back.',
        '$DefaultCreateViewDescription': 'Please fill this form and click OK button to create a new {0} record. Click Cancel to return to the previous screen.'
    },
    DefaultCategoryDescriptions: {
        '$DefaultEditDescription': 'These are the fields of the {0} record that can be edited.',
        '$DefaultNewDescription': 'Complete the form. Make sure to enter all required fields.',
        '$DefaultReferenceDescription': 'Additional details about {0} are provided in the reference information section.'
    }
}

Web.DataViewResources.Actions = {
    Scopes: {
        'Grid': {
            'Select': {
                HeaderText: 'Select'
            },
            'Edit': {
                HeaderText: 'Edit'
            },
            'Delete': {
                HeaderText: 'Delete',
                Confirmation: 'Delete?'
            },
            'Select': {
                HeaderText: 'Select'
            },
            'Duplicate': {
                HeaderText: 'Duplicate'
            },
            'New': {
                HeaderText: 'New'
            },
            'BatchEdit': {
                HeaderText: 'Batch Edit',
                CommandArgument: {
                    'editForm1': {
                        HeaderText: 'Batch Edit (Form)'
                    }
                }
            }
        },
        'Form': {
            'Edit': {
                HeaderText: 'Edit'
            },
            'Delete': {
                HeaderText: 'Delete',
                Confirmation: 'Delete?'
            },
            'Cancel': {
                HeaderText: 'Close',
                WhenLastCommandName: {
                    'Duplicate': {
                        HeaderText: 'Cancel'
                    },
                    'Edit': {
                        HeaderText: 'Cancel'
                    },
                    'New': {
                        HeaderText: 'Cancel'
                    }

                }
            },
            'Update': {
                HeaderText: 'OK',
                WhenLastCommandName: {
                    'BatchEdit': {
                        HeaderText: 'Update Selection',
                        Confirmation: 'Update?'
                    }
                }
            },
            'Insert': {
                HeaderText: 'OK'
            }
        },
        'ActionBar': {
            _Self: {
                'Actions': {
                    HeaderText: 'Actions'
                },
                'Report': {
                    HeaderText: 'Report'
                },
                'Record': {
                    HeaderText: 'Record'
                }
            },
            'New': {
                HeaderText: 'New {0}',
                Description: 'Create a new {0} record.',
                HeaderText2: 'New',
                VarMaxLen: 15
            },
            'Edit': {
                HeaderText: 'Edit'
            },
            'Delete': {
                HeaderText: 'Delete',
                Confirmation: 'Delete?'
            },
            'ExportCsv': {
                HeaderText: 'Download',
                Description: 'Download items in CSV format.'
            },
            'ExportRowset': {
                HeaderText: 'Export to Spreadsheet',
                Description: 'Analyze items with spreadsheet<br/>application.'
            },
            'ExportRss': {
                HeaderText: 'View RSS Feed',
                Description: 'Syndicate items with an RSS reader.'
            },
            'Update': {
                HeaderText: 'Save',
                Description: 'Save changes to the database.'
            },
            'Insert': {
                HeaderText: 'Save',
                Description: 'Save new record to the database.'
            },
            'Cancel': {
                HeaderText: 'Cancel',
                WhenLastCommandName: {
                    'Edit': {
                        HeaderText: 'Cancel',
                        Description: 'Cancel all record changes.'
                    },
                    'New': {
                        HeaderText: 'Cancel',
                        Description: 'Cancel new record.'
                    }
                }
            },
            'Report': {
                HeaderText: 'Report',
                Description: 'Render a report in PDF format'
            },
            'ReportAsPdf': {
                HeaderText: 'PDF Document',
                Description: 'View items as Adobe PDF document.<br/>Requires a compatible reader.'
            },
            'ReportAsImage': {
                HeaderText: 'Multipage Image',
                Description: 'View items as a multipage TIFF image.'
            },
            'ReportAsExcel': {
                HeaderText: 'Spreadsheet',
                Description: 'View items in a formatted<br/>Microsoft Excel spreadsheet.'
            }
        },
        'Row': {
            'Update': {
                HeaderText: 'Save',
                WhenLastCommandName: {
                    'BatchEdit': {
                        HeaderText: 'Update Selection',
                        Confirmation: 'Update?'
                    }
                }
            },
            'Insert': {
                HeaderText: 'Insert'
            },
            'Cancel': {
                HeaderText: 'Cancel'
            }
        }
    }
}

if (typeof (Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();
