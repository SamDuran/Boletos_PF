import "./Home.style.css"
import Sidebar from "./subcomponents/sidebar";
import React, { useState } from 'react';
import { categorias } from '../api/categorias.tsx'
import { eventos } from '../api/eventos.tsx'
import SearchBar from "./subcomponents/searchbar.tsx";
import Body from "./subcomponents/body.tsx"

function Home() {
    const [categories, setCategories] = useState<CategoriaEventos[]>([]);
    const [events, setEventos] = useState<Eventos[]>([]);
    const [allEvents, setAllEventos] = useState<Eventos[]>([]);



    const [categoryId, setCategory] = React.useState<number>(0)
    const handleClick = (numero: number) => {
        setCategory(numero);
    };
    
    const [search, setSearch] = React.useState<string>("")
    const handleSearch = (input: string) => {
        setSearch(input);
        setEventos(allEvents.filter((evento)=>{
            return evento.nombreEvento.includes(search) || evento.descripcion.includes(search)
        }));
    };

    React.useEffect(() => {
        categorias
            .getAll().then((r) => {
                setCategories(r.data);
            })
            .catch((e) => {
                console.error(e);
            });
        eventos
            .getAllByCategory(categoryId).then((r) => {
                setEventos(r.data);
            })
            .catch((e) => {
                console.error(e);
            });
        eventos
            .getAll().then((r) => {
                setAllEventos(r.data);
            })
            .catch((e) => {
                console.error(e);
            });
    }, [categoryId]);

    return (
        <>
            <article className="article-header">
                <img className="profile-icon" src="/src/components/resources/profileIcon.png"></img>
                <header className="header">
                    <img src="/src/components/resources/BoletosPF_Icon.png" className="header-logo" />
                    <h1 className="header-title">Ticket tracker</h1>
                    <h4>¡Compra tus boletos sin límites, sin estrés!</h4>
                </header>
            </article>
            <section>
                <div className="page-body">
                    <Sidebar lista={categories} onCategoryClick={handleClick} />
                    <div className="body-col">
                        <SearchBar OnSearch={handleSearch}/>
                        <Body lista={events} />
                    </div>
                </div>
            </section>
        </>
    );
}

export default Home