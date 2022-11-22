import { Injectable } from '@angular/core';
import * as FileSaver from 'file-saver';
import * as XLSX from 'xlsx';

const EXCEL_TYPE = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8';
const EXCEL_EXTENSION = '.xlsx';

@Injectable({
  providedIn: "root",
})
export class ExcelService {

  constructor() { }

  public exportAsExcelCustomHeaders(json: any[], header: string[][], excelFileName: string): void {
    //Creacion de libro
    const workbook = XLSX.utils.book_new();
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet([]);
    worksheet['!cols']=[{ wpx : 100 },{ wpx : 200 },{ wpx : 100 },{ wpx : 200 },{ wpx : 100 }];
    //Se agregan los encabezados
    XLSX.utils.sheet_add_aoa(worksheet, header,);

    //Comenzamos en la segunda fila y se omiten los encabezados
    XLSX.utils.sheet_add_json(worksheet, json, { origin: 'A2', skipHeader: true,});
    XLSX.utils.book_append_sheet(workbook, worksheet, 'data');

    excelFileName = 'Reporte_' + excelFileName.replace(" ", "_") + EXCEL_EXTENSION;
    XLSX.writeFile(workbook, excelFileName);
  }

  public exportAsExcelFile(json: any[], excelFileName: string): void {
    const worksheet: XLSX.WorkSheet = XLSX.utils.json_to_sheet(json);
    const workbook: XLSX.WorkBook = { Sheets: { 'data': worksheet }, SheetNames: ['data'] };

    const excelBuffer: any = XLSX.write(workbook, { bookType: 'xlsx', type: 'buffer' });
    this.saveAsExcelFile(excelBuffer, excelFileName);
  }

  private saveAsExcelFile(buffer: any, fileName: string): void {
    const data: Blob = new Blob([buffer], {
      type: EXCEL_TYPE
    });
    FileSaver.saveAs(data, fileName + '_export_' + new Date().getTime() + EXCEL_EXTENSION);
  }
}
