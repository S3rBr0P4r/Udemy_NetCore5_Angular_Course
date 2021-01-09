import { HttpClient, HttpParams } from "@angular/common/http";
import { map } from "rxjs/operators";
import { PaginatedResult } from "../models/pagination";

export function getPaginatedResults<T>(url, params, httpClient: HttpClient) {
  const paginatedResults: PaginatedResult<T> = new PaginatedResult<T>();
  return httpClient.get<T>(url, { observe: 'response', params }).pipe(
    map(response => {
      paginatedResults.result = response.body;
      if (response.headers.get('Pagination') !== null) {
        paginatedResults.pagination = JSON.parse(response.headers.get('Pagination'));
      }
      return paginatedResults;
    })
  );
}

export function getPaginationHeaders(pageNumber: number, pageSize: number) {
  let params = new HttpParams();

  params = params
    .append('pageNumber', pageNumber.toString())
    .append('pageSize', pageSize.toString());

  return params;
}
