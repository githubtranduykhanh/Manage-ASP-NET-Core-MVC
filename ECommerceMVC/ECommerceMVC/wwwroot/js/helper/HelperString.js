import moment from 'https://cdn.jsdelivr.net/npm/moment@2.30.1/+esm'


export const formatDatatime = (datatime) => moment(datatime).format("YYYY-MM-DD HH:mm");