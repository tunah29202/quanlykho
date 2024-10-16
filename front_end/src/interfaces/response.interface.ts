export type APIResponse<T> = {
    page: any;
    size: any;
    total: any;
    code: string;
    message: string;
    data?: T;
  };
  
  export type MetaResponse = {
    page: number;
    size: number;
    total: number;
  };
  