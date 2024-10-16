<script lang="ts">
    import { toast } from 'vue3-toastify';
    import 'vue3-toastify/dist/index.css';

    const showToast = (message: string, type: 'success' | 'error') => {
        toast[type](message, {
            autoClose: 3000,
        });
    };

    const push = (params: { message: string; type: 'success' | 'error' }) => {
        const { message, type } = params;
        showToast(message, type);
    };

    const handleApiResponse = (response: { message: string; code: number }) => {
        const message = response.message;
        const type = response.code === 0 ? 'success' : 'error';
        push({ message, type });
    };

    export const useToast = {
        push,
        handleApiResponse,
    };
</script>
