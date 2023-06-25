import "./sidebar.css"

export default function Sidebar({ lista, onCategoryClick}: { lista: CategoriaEventos[], onCategoryClick:(categoryId:number) => void }) {
    
    const handleChange = (categoriaId: number) => {
        onCategoryClick(categoriaId)
    }
    
    return (
        <>
            <div className="side-bar-bg">
                <label className="nav-label"onClick={() =>handleChange(0)}>
                    Categorias
                </label>
                {lista.map((category) => (
                    <span key={category.categoriaId} onClick={() =>handleChange(category.categoriaId)} className="nav-item">
                        {category.categoria}

                    </span>
                ))}
            </div>
        </>
    );
}
