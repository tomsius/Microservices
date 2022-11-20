import './App.css';
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { Navigation } from './components/Navigation';
import { Home } from './components/Home';
import { NotFound } from './components/NotFound';
import { Students } from './components/sudents_components/Students';
import { Courses } from './components/courses_components/Courses';

function App() {
  return (
    <BrowserRouter>
      <Navigation />

      <Routes>
        <Route path='/' element={<Home />} />
        <Route path='/students' element={<Students />} />
        <Route path='/courses' element={<Courses />} />

        <Route path="*" element={<NotFound />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
