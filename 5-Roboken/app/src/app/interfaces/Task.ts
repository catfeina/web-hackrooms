export interface TaskResponse {
    taskCode: number;
    title: string;
    description: string;
    status: string;
    comments: CommentResponse[];
}

export interface CommentResponse {
    comment: string;
    userName: string;
}