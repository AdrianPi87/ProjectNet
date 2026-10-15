export interface LoginRequestDto { username: string; password: string; }
export interface LoginResponseDto { token: string; username: string; role: string; }
export interface UserAccountDto { id: string; username: string; role: string; createdAtUtc: string; }
