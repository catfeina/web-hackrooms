export interface Response<T> {
    success: number;
    message: string;
    data: T;
}