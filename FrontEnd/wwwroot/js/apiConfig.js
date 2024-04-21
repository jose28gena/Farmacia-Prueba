// apiConfig.js
const API_BASE_URL = 'https://localhost:7126';

const API_URLS = {
    // Login
    login: `${API_BASE_URL}/Login`,

    // Medicamento
    medicamentos: `${API_BASE_URL}/Medicamento`,
    medicamentoPorId: (id) => `${API_BASE_URL}/Medicamento/${id}`,
    vistaMedicamentos: `${API_BASE_URL}/Medicamento/vmedicamentos`,
    ObtenerMedicamentosPaginados: `${API_BASE_URL}/Medicamento/ObtenerMedicamentosPaginados`,

    // Usuario
    usuarios: `${API_BASE_URL}/Usuario`,
    usuarioPorId: (id) => `${API_BASE_URL}/Usuario/${id}`,
    ObtenerUsuariosPaginados: `${API_BASE_URL}/Usuario/ObtenerUsuariosPaginados`,
    //Formas Farmaceuticas
    formasFarmaceuticas: `${API_BASE_URL}/FormaFarmaceutica`,
    formaFarmaceuticaPorId: (id) => `${API_BASE_URL}/FormaFarmaceutica/${id}`,
};