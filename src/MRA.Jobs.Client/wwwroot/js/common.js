'use strict';

/**
 * @see HeaderSearch
 * @see ModuleSearch
 * @see Sidebar
 * @see SidebarPanel
 * @see SwitcherGroup
 * @see CheckboxToggle
 * @see Tag
 * @see ChartProgressCircle
 * @see Charts
 * @see Echart
 * @see Checkboxes
 * @see ImageUpload
 * @see ProfileUpload
 * @see CalendarInline
 * @see CalendarFull
 * @see Inbox
 * @see Chat
 * @see Todo
 * @see Timeline
 * @see Pagination
 * @see InputSelects
 * @see InputTags
 * @see Modal
 * @see DropdownSelect
 * @see AddProduct
 * @see OrderTabs
 * @see ScrollTo
 */

const isTouchDevices = ('ontouchstart' in window) || (window.DocumentTouch && document instanceof window.DocumentTouch);
const body = document.querySelector('body');

const delay = time => {
    return new Promise(resolve => setTimeout(resolve, time));
};

const triggerEvent = (typeEvent, elem, bubbles = true) => {
    let event;

    if (typeEvent === 'click') {
        event = new MouseEvent('click', {
            bubbles,
            cancelable: true,
            view: window
        });
    }

    const canceled = !elem.dispatchEvent(event);
};

const themeStyle = (a, b, cssVariable = true) => {
    const theme = document.querySelector('html').getAttribute('data-theme');

    const getStyle = color => {
        return getComputedStyle(document.documentElement).getPropertyValue(color);
    };

    if (typeof b !== 'undefined' && b !== '') {
        if (theme === 'light') {
            return cssVariable ? getStyle(a) : a;
        }

        if (theme === 'dark') {
            return cssVariable ? getStyle(b) : b;
        }
    }

    if ((typeof b === 'undefined' || b === '') && !cssVariable) {
        return a;
    }

    return getStyle(a);
};

const imgLoaded = imagesLoaded(document.querySelector('body'));
const timeline = new Timeline();

$(function () {
    if (isTouchDevices) document.querySelector('html').classList.add('is-touch');

    imgLoaded.on('always', () => {
        setTimeout(() => {
            body.classList.add('load');
            chart();
            timeline.init();
        }, 300);
    });

    window.addEventListener('resize', () => {
        timeline.resize();
    });

    document.querySelectorAll('[data-simplebar]').forEach(el => {
        const simpleBar = new SimpleBar(el);

        simpleBar.recalculate();
    });

    document.querySelectorAll('.js-dropdown-select').forEach(el => {
        const element = el;

        new DropdownSelect(element);
    });

    document.querySelectorAll('.items-more__button').forEach(el => {
        const button = el;

        if (button) {
            button.addEventListener('click', () => button.focus());
        }
    });

    new HeaderSearch();
    new ModuleSearch();
    new Sidebar();
    new SidebarPanel();
    new SwitcherGroup();
    new CheckboxToggle();
    new Tag();
    tooltip();
    form();
    formEditor();
    map();
    new Checkboxes();
    new ImageUpload();
    new ProfileUpload();
    new CalendarInline('#calendarOne');
    new CalendarFull();
    new Inbox();
    new Chat();
    new Todo();
    new Pagination();
    new InputSelects('.js-input-select');
    new InputTags('.js-input-tags');
    new Modal();
    new AddProduct();
    new OrderTabs();
    new ScrollTo();
    scrolls();
}());
