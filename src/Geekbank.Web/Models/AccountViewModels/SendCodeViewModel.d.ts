declare module server {
	interface sendCodeViewModel {
		selectedProvider: string;
		providers: any[];
		returnUrl: string;
		rememberMe: boolean;
	}
}
